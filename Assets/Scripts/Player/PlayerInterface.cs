using Game;

public interface IItemUseable
{
    void UseItemByType(WeaponType type);
    void EndUseItemByType(WeaponType type);
}