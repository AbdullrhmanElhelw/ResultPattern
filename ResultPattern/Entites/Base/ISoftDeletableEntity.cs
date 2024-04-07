namespace ResultPattern.Entites.Base;

public interface ISoftDeletableEntity
{
    bool IsDeleted { get; }
    DateTime? DeletedOnUtc { get; }
}