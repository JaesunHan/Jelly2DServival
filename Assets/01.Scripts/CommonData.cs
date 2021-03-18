namespace CommonData
{
    public enum EDir
    { 
        Dir_None = 0,

        Dir_Left = -1, 
        Dir_Right = 1,
    }

    public enum EPlayerState
    {
        Nonne = -1,

        Idle = 0,

        Run = 1,

        Death = 2,

        Stun = 3,

        Attack_Sword = 4,

        Attack_Bow = 5,

        Attack_Magic = 6,

        Skill_Sword = 7,

        Skill_Bow = 8,

        Skill_Magic = 9,
    }
}