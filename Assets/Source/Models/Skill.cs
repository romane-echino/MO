public class Skill
{
    public string Name;
    public string Icon;
    public string[] Pattern; // = new string[] { "1_1" };
    public float Cooldown;

    public Skill(string Name, string Icon, string[] Pattern, float Cooldown){
        this.Name = Name;
        this.Icon = Icon;
        this.Pattern = Pattern;
        this.Cooldown = Cooldown;
    }
}
