using Microsoft.Net.Http.Headers;

namespace API;

public class AppUser
{
public int Id{get; set;}
public string UserName{get; set;}
public byte[] PasswordHash{get; set;}
public byte[] PasswordSalt{get; set;}
}
