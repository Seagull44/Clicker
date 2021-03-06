﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Collections;

public class GoldFarmerType
{
    public int count;
    public int goldMultiplier;
    public int upgradegps;
    public int cost;

    public GoldFarmerType(int multiplier, int count, int upgradegps, int cost)
    {
        goldMultiplier = multiplier;
        this.count = count;
        this.upgradegps = upgradegps;
    }

    public float gps()
    {
        return count * (goldMultiplier * upgradegps);
    }

    virtual public bool canAdd()
    {
        return true;
    }

    public void add()
    {
        if (canAdd())
        {
            count++;
        }
    }

    virtual public string description()
    {
        return count.ToString();
    }
}

public class PriestType : GoldFarmerType
{
    public PriestType() : base(1,0,0,120) { }

    override public string description()
    {
        return "Priests - " + base.description();
    }
}

public class MonkType : GoldFarmerType
{
    public int candleCount;

    public MonkType() : base(100,0,0,1000) { }

    override public bool CanAdd()
    {
        return count < candleCount * 5;
    }

    override public string Description()
    {
        return "Monks - " + base.description();
    }
}

public enum Currency {
    Coin,
    Gem
}

public class Transaction {
    public Date date;
    public Currency currency;
    public int change;

    public Transaction(Currency currency, int change) {
        this.date = new Date();
        this.currency = currency;
        this.change = change;
    }
}

public class CurrencyAccount {
    public Currency currency;
    public int ammount;
}

public class Wallet {
    private CurrencyAccount[] accounts = [
        new CurrencyAccount { currency = Currency.Coin, ammount = 0 },
        new CurrencyAccount { currency = Currency.Gem, ammount = 0 },
    ];

    public List<Transaction> transactions = new List<Transaction>();
    
    public void Add(Currency currency, int count) {
        let transaction = new Transaction(currency, count);
        this.transactions.Add(transaction);
        switch (currency) {
            case Currency.Coin:
                this.AddCoin(count);
                break;
            case Currency.Gem:
                this.AddGem(count);
                break;
        }
    }

    private void AddCoin(int count) {
        this.accounts[0].ammount = this.accounts[0].ammount + count;
    }

    private void AddGem(int count) {
        this.accounts[1].ammount = this.accounts[1].ammount + count;
    }
}

public class Script1 : MonoBehaviour
{
    public Text scoreText;
    public Text priestcountText;
    public Text monkcountText;
    public Text gemText;
    public Button[] upgradebuttons;
    public GameObject upgradepan;
    public float[] PrayTime;
    private int score = 0;
    private int bonus = 1;
    public int priestscount, priestbonus = 1;
    public int priestupdate;

    public GameObject nempan;
    public int[] upgradeCosts;
    public int[] upgradeBonuses;
    public Text[] upgradebttnstext;
    public GameObject inventorypan;
    private int gems = 400000;
    private int gembonus = 1;
    public int altar;
    public int remains;

    public int icon;
    public Button[] inventorybuttons;
    public int candle;
    public int monkcount, monkbonus = 100;
    public int monkupgrade;
    public int[] inventorycosts;

    public GameObject Tamplepan;

    public GoldFarmerType[] goldFarmerTypes;

    public Script1()
    {
        var monkType = new MonkType();
        var priestType = new PriestType();
        goldFarmerTypes = new GoldFarmerType[] { monkType, priestType };
    }

    private void Start()
    {
        StartCoroutine(Bonuspersec());
        StartCoroutine(Gembonuspersec());
        StartCoroutine(Monkbonuspersec());
    }
    private void Update()                                                   // не забудь взнати чи нагромадження в цьому методі не призведе до глюків
    {
        gemText.text = gems + "G";
        scoreText.text = score + "$";
        if (priestscount <= 0)
        {
            priestcountText.text = " ";
        }
        else
        {
            priestcountText.text = "Priests - " + priestscount;
        }
        if (monkcount <= 0)
        {
            monkcountText.text = " ";
        }
        else
        {
            monkcountText.text = "Monks - " + monkcount;
        }

        if (monkcount >= candle * 5)
        {
            upgradebuttons[1].interactable = false;
        }
        else
        {
            upgradebuttons[1].interactable = true;
        }

        if (priestscount <= 0)
        {
            upgradebuttons[3].interactable = false;
        }
        else
        {
            upgradebuttons[3].interactable = true;
        }
        if (priestupdate >= 3)
        {
            upgradebuttons[3].interactable = false;
        }
        if (monkcount <= 0)
        {
            upgradebuttons[4].interactable = false;
        }
        else
        {
            upgradebuttons[4].interactable = true;
        }
        if (monkupgrade >= 3)
        {
            upgradebuttons[4].interactable = false;
        }

    }


    IEnumerator Gembonuspersec()                                                   // щосекунди добавляє гембонус
    {
        while (true)
        {
            gems += (altar * gembonus);
            yield return new WaitForSeconds(300);
        }

    }

    public void remainstoaltar(int index)
    {
        if (gems >= inventorycosts[3])
        {
            gems -= inventorycosts[3];
            gembonus++;
            inventorycosts[3] *= 2;
            upgradebttnstext[8].text = "Buy new remains\n" + inventorycosts[3] + "G";
        }
    }

    public void priestupdaterule(int index)                                     // апгрейд прістів

    {

        if (score >= upgradeCosts[4])
        {
            score -= upgradeCosts[4];
            priestupdate++;
            upgradeCosts[4] *= 5;
            upgradebttnstext[6].text = "Upgrade priests\n" + upgradeCosts[4] + "$";
        }
        else
        {
            StartCoroutine(nem(nempan));                                        // запускає каротіну для панельки з надписом що не достатньо коштів
        }
        if (priestupdate > 0)
        {
            priestbonus *= (50 * priestupdate);
        }

        if (priestupdate >= 3)
        {
            upgradebttnstext[6].text = "MAX upgrade";
        }



    }

    public void Monkupgraderule(int index)                                          // прокачка монків

    {

        if (score >= upgradeCosts[5])
        {
            score -= upgradeCosts[5];
            monkupgrade++;
            upgradeCosts[5] *= 3;
            upgradebttnstext[7].text = "Upgrade monks\n" + upgradeCosts[5] + "$";
        }
        else
        {
            StartCoroutine(nem(nempan));                                        // запускає каротіну для панельки з надписом що не достатньо коштів
        }
        if (monkupgrade > 0)
        {
            monkbonus *= (50 * monkupgrade);
        }

        if (monkupgrade >= 3)
        {
            upgradebttnstext[7].text = "MAX upgrade";
        }



    }

    public void superbonus(int index)                                                  // купівля ікони
    {
        if (gems >= inventorycosts[2])
        {
            gems -= inventorycosts[2];
            inventorycosts[2] += inventorycosts[2] * 3;
            icon++;
            upgradebttnstext[5].text = "Icon\n" + inventorycosts[2] + "G";
        }
        if (icon > 0)
        {
            bonus = bonus * icon * 40;
        }
        else
        {
            StartCoroutine(nem(nempan));                                        // запускає каротіну для панельки з надписом що не достатньо коштів
        }
    }


    public void inventory()                                                     // відкриває\закриває інвентар

    {
        inventorypan.SetActive(!inventorypan.activeSelf);
    }


    public void inventorybuy(int index)                                     // відповідає за купівлю всередені інвентаря
    {
        if (score >= inventorycosts[0])
        {
            score -= inventorycosts[0];
            altar++;
            inventorybuttons[index].interactable = false;
        }
        else
        {
            StartCoroutine(nem(nempan));                                        // запускає каротіну для панельки з надписом що не достатньо коштів
        }
    }


    public void upgrade()                                                       // кнопка апгрейд знизу зправа
    {
        upgradepan.SetActive(!upgradepan.activeSelf);
    }
    public void upgradepanplusbonus(int index)                                  // робота кнопки лвлап
    {
        if (score >= upgradeCosts[index])
        {
            bonus += upgradeBonuses[index];
            score -= upgradeCosts[index];
            upgradeCosts[index] *= 2;
            upgradebttnstext[index].text = "Lvlup\n" + upgradeCosts[index] + "$";
        }
        else
        {
            StartCoroutine(nem(nempan));                                        // запускає каротіну для панельки з надписом що не достатньо коштів
        }


    }

    public void HireMonk(int index)                                             // відповідає за купівлю монків
    {



        if (score >= upgradeCosts[index])
        {
            monkcount++;
            score -= upgradeCosts[index]
;
            upgradeCosts[index] *= 10;
            upgradebttnstext[3].text = "Hire monk\n" + upgradeCosts[index] + "$";
        }
        else
        {
            StartCoroutine(nem(nempan));                                        // запускає каротіну для панельки з надписом що не достатньо коштів
        }


    }

    public void Candlplus(int index)                                         // купівля свічок
    {
        if (gems >= upgradeCosts[index])
        {
            candle++;
            gems -= upgradeCosts[index];
            upgradeCosts[index] *= 10;
            upgradebttnstext[4].text = "Candle\n" + upgradeCosts[index] + "G";
        }
        else
        {
            StartCoroutine(nem(nempan));                                        // запускає каротіну для панельки з надписом що не достатньо коштів
        }
    }



    public void Hirepriest(int index)                                             // метод для найму прістів
    {
        if (score >= upgradeCosts[index])
        {
            priestscount++;
            score -= upgradeCosts[index];
            upgradeCosts[index] *= 2;
            upgradebttnstext[index].text = "Hire priest\n" + upgradeCosts[index] + "$";
        }
        else
        {
            StartCoroutine(nem(nempan));                                        // запускає каротіну для панельки з надписом що не достатньо коштів
        }
    }

    public void startPraytimer(int index)                                           // старт молитви
    {

        int cost = 3 * priestscount;
        if (priestscount > 0)
            upgradebttnstext[2].text = "Pray\n" + cost + "$";

        if (score >= cost)
        {
            StartCoroutine(PrayTimer(PrayTime[index], index));
            score -= cost;
        }
        else
        {
            StartCoroutine(nem(nempan));                                        // запускає каротіну для панельки з надписом що не достатньо коштів
        }

    }

    IEnumerator Bonuspersec()                                                   // щосекунди добавляє бонус
    {
        while (true)
        {
            score += (priestscount * priestbonus);
            yield return new WaitForSeconds(1);
        }

    }

    IEnumerator PrayTimer(float timer, int index)                               // умови дії молитви
    {
        upgradebuttons[index].interactable = false;
        if (index == 0 && priestscount > 0)
        {
            priestbonus *= 3;
            yield return new WaitForSeconds(timer);
            priestbonus /= 3;
        }
        upgradebuttons[index].interactable = true;

    }

    IEnumerator nem(GameObject nempan)                                          // так діє вспливашка при недостатніх коштах
    {

        nempan.SetActive(true);
        yield return new WaitForSeconds(1);
        nempan.SetActive(false);

    }

    IEnumerator Monkbonuspersec()                                                  // щосекунди добавляє бонус
    {
        while (true)
        {
            score += (monkcount * monkbonus);
            yield return new WaitForSeconds(1);
        }

    }

    public void Tample()                                                     // відкриває\закриває інвентар

    {
        Tamplepan.SetActive(!Tamplepan.activeSelf);
    }

    public void OnClick()
    {
        score += bonus;

    }


}