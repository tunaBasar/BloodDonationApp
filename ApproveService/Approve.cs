namespace ApproveService.Models
{
    public class ApproveRequest
{
    public string UserDonorTc { get; set; } = null!;
    public int RequestId { get; set; }
    public string? UserRequesterTc { get; set; }
}

public class ApproveResponse
{
    public int Id { get; set; }
    public string UserDonorTc { get; set; } = null!;
    public int? RequestId { get; set; }
    public string? UserRequesterTc { get; set; }
    public bool? IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string Status => IsApproved switch
    {
        null => "Bekliyor",
        true => "Onaylandı",
        false => "Reddedildi"
    };
}

public class Approve
{
    public int Id { get; set; }
    public string UserDonorTc { get; set; } = null!;
    public int? RequestId { get; set; } 
    public string? UserRequesterTc { get; set; }
    public bool? IsApproved { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
}
}
