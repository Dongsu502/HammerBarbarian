using Game;

public interface IItemUseable
{
    void UseItemByType(WeaponType type);
    void EndUseItemByType(WeaponType type);
}

public interface IAttackable
{
    void AttackByType(WeaponType type);
}