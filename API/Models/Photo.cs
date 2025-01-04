using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("Photo")]
public class Photo
{
	public int ID { get; set; }
	public required string URL { get; set; }
	public bool IsMain { get; set; }
	public string? PublicID { get; set; }

	//Navigation Properties
	public int UserID { get; set; }
	public User User { get; set; } = null!;
}