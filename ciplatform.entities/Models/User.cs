using System;
using System.Collections.Generic;

namespace ciplatform.entities.Models;

public partial class User
{
    public long UserId { get; set; }

    public string? FirstName { get; set; }

    public string? SecondName { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public long PhoneNumber { get; set; }

    public string? Avatar { get; set; }

    public string? WhyIVolunteer { get; set; }

    public string? EmployeeId { get; set; }

    public string? Department { get; set; }

    public long CityId { get; set; }

    public long CountryId { get; set; }

    public string? ProfileText { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? Title { get; set; }

    public bool? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? Availability { get; set; }

    public string? Manager { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual ICollection<ContactU> ContactUs { get; } = new List<ContactU>();

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<EnableUserStatus> EnableUserStatuses { get; } = new List<EnableUserStatus>();

    public virtual ICollection<FavouritMission> FavouritMissions { get; } = new List<FavouritMission>();

    public virtual ICollection<MissionApplication> MissionApplications { get; } = new List<MissionApplication>();

    public virtual ICollection<MissionInvite> MissionInviteFromUsers { get; } = new List<MissionInvite>();

    public virtual ICollection<MissionInvite> MissionInviteToUsers { get; } = new List<MissionInvite>();

    public virtual ICollection<MissionRating> MissionRatings { get; } = new List<MissionRating>();

    public virtual ICollection<Story> Stories { get; } = new List<Story>();

    public virtual ICollection<StotyInvite> StotyInviteFromUsers { get; } = new List<StotyInvite>();

    public virtual ICollection<StotyInvite> StotyInviteToUsers { get; } = new List<StotyInvite>();

    public virtual ICollection<Timesheet> Timesheets { get; } = new List<Timesheet>();

    public virtual ICollection<UserSkill> UserSkills { get; } = new List<UserSkill>();

    public virtual ICollection<Userpermission> Userpermissions { get; } = new List<Userpermission>();
}
