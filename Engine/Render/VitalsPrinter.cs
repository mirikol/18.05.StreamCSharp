using Spectre.Console;

public enum VitalsPrinterMode
{
    Base,
    Extra,
    Weapon
}

public class VitalsPrinter : IPrinter
{
    private const string _destroyedBodyPartStatus = "УНИЧТОЖЕНО";
    private const string _criticalBodyPartStatus = "КРИТ";
    private const string _hurtBodyPartStatus = "ПОВРЕЖДЕНО";
    private const string _normalBodyPartStatus = "НОРМА";

    private const char _healthBarFillChar = '█';

    private LiveDisplayContext _displayContext;
    private Layout _layout;

    private VitalsPrinterMode _mode = VitalsPrinterMode.Base;

    public void Initialize(LiveDisplayContext context, Layout layout)
    {
        _displayContext = context;
        _layout = layout;
    }

    public void Reset()
    {
        var text = new Text("No selected unit", new Style(Color.Gray, null, Decoration.Bold, null));
        var panel = new Panel(text).Header("Vitals").BorderColor(Color.White).Expand();
        _layout["Info"]["Vitals"].Update(panel);
        _displayContext.Refresh();
    }

    public void SwiitchPrintMode()
    {
        if (_mode == VitalsPrinterMode.Base)
        {
            _mode = VitalsPrinterMode.Extra;
        }
        else if (_mode == VitalsPrinterMode.Extra)
        {
            _mode = VitalsPrinterMode.Weapon;
        }
        else
        {
            _mode = VitalsPrinterMode.Base;
        }
    }

    public void Print(VitalsContext context)
    {
        var table = new Table().AddColumn("").AddColumn("").Border(TableBorder.None);
        var panel = new Panel(table).Header("Vitals").BorderColor(Color.White).Expand();

        var bodyParts = context.Unit.BodyParts;
        var health = 0;
        var maxHealth = 0;

        foreach (var bodyPart in bodyParts.Values)
        {
            health += bodyPart.Health;
            maxHealth += bodyPart.MaxHealth;
        }

        table.AddRow("Здоровье:", GetColoredText(GetHealthBar(health, maxHealth), GetStatus(health, maxHealth)) + $" | {health}/{maxHealth}");
        table.AddEmptyRow();

        foreach (var bodyPart in bodyParts)
        {
            table.AddRow($"{bodyPart.Key}:", GetColoredText(GetHealthBar(bodyPart.Value.Health, bodyPart.Value.MaxHealth), GetStatus(bodyPart.Value.Health, bodyPart.Value.MaxHealth)) + $" | {bodyPart.Value.Health}/{bodyPart.Value.MaxHealth}");
            table.AddEmptyRow();
        }

        table.AddRow($"= = = = =", "= = = = = = = = = = = = = = =");
        table.AddEmptyRow();

        foreach (var bodyPart in bodyParts.Values)
        {
            if (bodyPart is Arm arm)
            {
                if (arm.HasWeapon)
                {
                    if (_mode == VitalsPrinterMode.Base)
                    {
                        table.AddRow(arm.Weapon.Name + ':', $"Атака: {arm.Weapon.Attack}, Защита: {arm.Weapon.Defense}");
                    }
                    else if (_mode == VitalsPrinterMode.Extra)
                    {
                        table.AddRow(arm.Weapon.Name + ':', $"Скорость: {arm.Weapon.Speed}, Инициатива: {arm.Weapon.Initiative}");
                    }
                    else
                    {
                        table.AddRow(arm.Weapon.Name + ':', $"Базовый урон: {arm.Weapon.BaseDamage}");
                    }
                    table.AddEmptyRow();
                }
            }
        }

        foreach (var bodyPart in bodyParts.Values)
        {
            if (bodyPart.HasArmor)
            {
                if (_mode == VitalsPrinterMode.Base)
                {
                    table.AddRow(bodyPart.Armor.Name + ':', $"Атака: {bodyPart.Armor.Attack}, Защита: {bodyPart.Armor.Defense}");
                }
                else
                {
                    table.AddRow(bodyPart.Armor.Name + ':', $"Скорость: {bodyPart.Armor.Speed}, Инициатива: {bodyPart.Armor.Initiative}");
                }

                table.AddEmptyRow();
            }
        }

        _layout["Info"]["Vitals"].Update(panel);
        _displayContext.Refresh();
    }

    private string GetHealthBar(int health, int maxHealth)
    {
        int barLength = 8;
        int filled = (int)Math.Round((double)health / maxHealth * barLength);
        filled = Math.Max(0, Math.Min(filled, barLength));

        return new string(_healthBarFillChar, filled).Replace(_healthBarFillChar.ToString(), $"{_healthBarFillChar} ") + new string('_', barLength - filled).Replace("_", "_ ");
    }

    private string GetStatus(int health, int maxHealth)
    {
        return health <= 0 ? _destroyedBodyPartStatus : health < maxHealth * 0.6 ? health < maxHealth * 0.3 ? _criticalBodyPartStatus : _hurtBodyPartStatus : _normalBodyPartStatus;
    }

    private string GetColoredText(string text, string status)
    {
        return status switch
        {
            _destroyedBodyPartStatus => "[darkred]" + text + "[/]",
            _criticalBodyPartStatus => "[red]" + text + "[/]",
            _hurtBodyPartStatus => "[yellow]" + text + "[/]",
            _ => "[green]" + text + "[/]"
        };
    }
}
