using ApproveService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Approve Service API", Version = "v1" });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMauiApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();


app.UseCors("AllowMauiApp");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Approve Service API V1");
    });
}


app.MapPost("/approve", async (ApplicationDbContext db, ApproveRequest request) =>
{

    if (string.IsNullOrWhiteSpace(request.UserDonorTc))
        return Results.BadRequest("UserDonorTc is required");

    if (request.RequestId <= 0)
        return Results.BadRequest("Valid RequestId is required");


    var existingApprove = await db.Approves
        .FirstOrDefaultAsync(a => a.UserDonorTc == request.UserDonorTc && a.RequestId == request.RequestId);

    if (existingApprove != null)
        return Results.Conflict("Bu bağış talebi için zaten bir başvurunuz bulunmaktadır");

    var approve = new Approve
    {
        UserDonorTc = request.UserDonorTc,
        RequestId = request.RequestId,
        UserRequesterTc = request.UserRequesterTc,
        IsApproved = null,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = null
    };

    db.Approves.Add(approve);
    await db.SaveChangesAsync();

    return Results.Created($"/approve/{approve.Id}", approve);
})
.WithName("CreateApproveRequest")
.WithSummary("Bağış talebine katılma isteği oluştur");


app.MapGet("/approve/pending/{userDonorTc}", async (string userDonorTc, ApplicationDbContext db) =>
{
    var pendingApproves = await db.Approves
        .Where(a => a.UserDonorTc == userDonorTc && a.IsApproved == null)
        .OrderByDescending(a => a.CreatedAt)
        .ToListAsync();

    return Results.Ok(pendingApproves);
})
.WithName("GetPendingApproves")
.WithSummary("Kullanıcının onay bekleyen bağış isteklerini getir");


app.MapGet("/approve/history/{userDonorTc}", async (string userDonorTc, ApplicationDbContext db) =>
{
    var approveHistory = await db.Approves
        .Where(a => a.UserDonorTc == userDonorTc)
        .OrderByDescending(a => a.CreatedAt)
        .ToListAsync();

    return Results.Ok(approveHistory);
})
.WithName("GetApproveHistory")
.WithSummary("Kullanıcının bağış geçmişini getir");


app.MapPut("/approve/confirm/{id}", async (int id, ApplicationDbContext db) =>
{
    var approve = await db.Approves.FindAsync(id);
    if (approve is null)
        return Results.NotFound("Onaylanacak bağış talebi bulunamadı");

    if (approve.IsApproved != null)
        return Results.BadRequest("Bu bağış talebi zaten işlenmiş");

    approve.IsApproved = true;
    approve.UpdatedAt = DateTime.UtcNow;

    await db.SaveChangesAsync();

    return Results.Ok(new
    {
        Message = "Bağış talebi onaylandı",
        Approve = approve
    });
})
.WithName("ConfirmApprove")
.WithSummary("Bağış talebini onayla");

app.MapPut("/approve/reject/{id}", async (int id, ApplicationDbContext db) =>
{
    var approve = await db.Approves.FindAsync(id);
    if (approve is null)
        return Results.NotFound("Reddedilecek bağış talebi bulunamadı");

    if (approve.IsApproved != null)
        return Results.BadRequest("Bu bağış talebi zaten işlenmiş");

    approve.IsApproved = false;
    approve.UpdatedAt = DateTime.UtcNow;
    approve.RequestId = null;

    await db.SaveChangesAsync();

    return Results.Ok(new
    {
        Message = "Bağış talebi reddedildi",
        Approve = approve
    });
})
.WithName("RejectApprove")
.WithSummary("Bağış talebini reddet");


app.MapGet("/approve/{id}", async (int id, ApplicationDbContext db) =>
{
    var approve = await db.Approves.FindAsync(id);
    if (approve is null)
        return Results.NotFound();

    return Results.Ok(approve);
})
.WithName("GetApproveById")
.WithSummary("ID'ye göre bağış talebini getir");


app.MapDelete("/approve/{id}", async (int id, ApplicationDbContext db) =>
{
    var approve = await db.Approves.FindAsync(id);
    if (approve is null)
        return Results.NotFound();

    if (approve.IsApproved != null)
        return Results.BadRequest("İşlenmiş bağış talepleri silinemez");

    db.Approves.Remove(approve);
    await db.SaveChangesAsync();

    return Results.Ok(new { Message = "Bağış talebi silindi" });
})
.WithName("DeleteApprove")
.WithSummary("Bağış talebini sil");

app.MapGet("/approve/incoming/{requesterTc}", async (string requesterTc, ApplicationDbContext db) =>
{
    var incomingRequests = await db.Approves
        .Where(a => a.UserRequesterTc == requesterTc && a.IsApproved==null)
        .OrderByDescending(a => a.CreatedAt)
        .ToListAsync();

    var dtoList = incomingRequests.Select(a => new ApproveDto
    {
        Id=a.Id,
        RequesterTc = a.UserRequesterTc ?? "",
        UserDonorTc = a.UserDonorTc,
        RequestId = a.RequestId ?? 0,
        CreatedAt = a.CreatedAt,
        UpdatedAt = a.UpdatedAt,
        IsApproved = a.IsApproved
    }).ToList();

    var response = new Response<List<ApproveDto>>
    {
        Success = true,
        Data = dtoList
    };

    return Results.Ok(response);
});


app.Run();

public record ApproveRequest(string UserDonorTc, int RequestId, string? UserRequesterTc = null);