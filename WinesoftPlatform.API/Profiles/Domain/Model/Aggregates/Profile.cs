using WinesoftPlatform.API.Profiles.Domain.Model.ValueObjects;

namespace WinesoftPlatform.API.Profiles.Domain.Model.Aggregates;

/// <summary>
///     Profile Aggregate Root
/// </summary>
/// <remarks>
///     This class represents the Profile aggregate root.
///     It contains the properties and methods to manage the profile information.
/// </remarks>
public partial class Profile
{
    public Profile()
    {
        Name = new PersonName();
        Email = new EmailAddress();
    }

    public Profile(string firstName, string lastName, string email)
    {
        Name = new PersonName(firstName, lastName);
        Email = new EmailAddress(email);
    }
    
    public int Id { get; }
    public PersonName Name { get; }
    public EmailAddress Email { get; }
    
    public string FullName => Name.FullName;
    public string EmailAddress => Email.Address;
}