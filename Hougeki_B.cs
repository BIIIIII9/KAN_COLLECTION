using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunZi
{
    public enum ExecBattleKinds
    {
        None,
        DayOnly,
        DayToNight,
        NightOnly,
        NithtToDay
    }//战斗类型
     //namespace Common.Enum V
    public enum BattleFormationKinds1
    {
        TanJuu = 1,
        FukuJuu,
        Rinkei,
        Teikei,
        TanOu
    }//阵型
    public enum BattleFormationKinds2
    {
        Doukou = 1,
        Hankou,
        T_Own,
        T_Enemy
    }//航向
    public enum DamageState
    {
        Normal,
        Shouha,
        Tyuuha,
        Taiha
    }
    //public class FormationDatas V
    public enum GetFormationKinds
    {
        AIR,
        HOUGEKI,
        RAIGEKI,
        MIDNIGHT,
        SUBMARINE
    }//攻击类型
    //Server_Common.Formats.Battle
    public enum BattleAtackKinds_Day
    {
        Normal,
        Bakurai,
        Gyorai,
        AirAttack,
        Laser,
        Renzoku,
        Sp1,
        Sp2,
        Sp3,
        Sp4
    }//昼战特殊攻击
    public enum BattleSeikuKinds
    {
        None,
        Kakuho,
        Yuusei,
        Ressei,
        Lost
    }//制空类型
    public class ShipData
    {
        public int Houg;
        public int Level;
        public int Luck;
        public int Avoid;
        public int Souko;
        public int Sakuteki;
        public int MaxHp;


        public int FitWeight_HitUp;

        public bool CanDayRenzoku;
        public bool CanDaySp4;

        public double ThisBattleBaseHoug;
        public double ThisBattleBaseHit;
        public double ThisBattlePosbilityDayRenzoku;
        public double ThisBattlePosbilityDaySp4;

        public int DeckId;
        public int ordernumberAttack;

        public bool EnemyFlag;

        public List<ItemData> Item_List;
        public List<PosShipDamage> PosibilityAttackDamage_List;
        public List<PosShipDamage> PosibilityDamaged_List;
        public ShipData()
        {
            Item_List = new List<ItemData>();
            PosibilityAttackDamage_List = new List<PosShipDamage>();
            PosibilityDamaged_List = new List<PosShipDamage>();
        }

    }
    public class ItemData
    {
        public int Houg;
        public int Houm;
        public int Sakuteki;
        public int Api_mapbattle_type3;
        public int slotLevel;
        public int onSlotNum;
    }
    public class BattleBaseData//针对昼战
    {
        public BattleSeikuKinds Seiku;
        public bool IsPossibleT_enemy;//航向     
        public readonly double valance1 = 5;
        public readonly double valance2 = 90;
        public readonly double valance3 = 1.3;
    }
    public class DeckData
    {
        public List<BattleFormationKinds1> Formation;//阵型
        public List<ShipData> Ship_List;
    }
    public class PosShipDamage
    {
        public int Hp;
        public double Posbility;
    }
    public class Contribution
    {
        int Hit;
        int AllDamage;
        int EffectDamage;
        int EnemyDestory;
        int Critical;
        int SpAttackSyosyo;
        int SpAttackDouble;
    }

    public class Hougeki_B
    {
        DeckData fDeck;
        DeckData eDeck;
        BattleBaseData FightParams;
        //需读取并填充舰队信息
        //构造函数初始化一个PosibilityEnemyDamage
        //成员函数
        public void DayHougekiBattle()
        {
            CalculateDayKindAttack();
            CalculateDayBaseHoug();
            CalculateDayBaseHit();
            DeckDirection();//航向
        }
        private void CalculateDayKindAttack()//计算每艘船弹观发动率
        {
            double AllDeckSakuteki = 0;//舰队索敌
            double[] ShipSumItemSakuteki = new double[fDeck.Ship_List.Count()];//单舰装备索敌
            int DeckIdx = 0;
            foreach (ShipData currentShip in fDeck.Ship_List)
            {
                AllDeckSakuteki += currentShip.Sakuteki;//舰队观测                               
                foreach (ItemData currentItem in currentShip.Item_List)
                {
                    if (currentItem.onSlotNum > 0 &&
                        (currentItem.Api_mapbattle_type3 == 10 || currentItem.Api_mapbattle_type3 == 10))//水观水爆
                    { AllDeckSakuteki += (int)Math.Sqrt((double)currentItem.onSlotNum) * currentItem.Sakuteki; }
                    ShipSumItemSakuteki[DeckIdx] += (double)currentItem.Sakuteki;
                }
                ++DeckIdx;
            }
            double AllDeckSpUp = (double)((int)(Math.Sqrt(AllDeckSakuteki) + AllDeckSakuteki * 0.1));
            BattleSeikuKinds Seiku = FightParams.Seiku;
            DeckIdx = 0;
            foreach (ShipData currentShip2 in fDeck.Ship_List)
            {
                int ShipLuckSpUp = (int)(Math.Sqrt((double)currentShip2.Luck) + 10.0);
                if (Seiku == BattleSeikuKinds.Kakuho)
                {
                    int Posbility = (int)((double)ShipLuckSpUp + 10.0 + (AllDeckSpUp + ShipSumItemSakuteki[DeckIdx] * 1.6) * 0.7);
                    if (currentShip2.CanDayRenzoku)
                        currentShip2.ThisBattlePosbilityDayRenzoku = (double)Posbility / 130.0;
                    else currentShip2.ThisBattlePosbilityDayRenzoku = 0;
                    if (currentShip2.CanDaySp4)
                        currentShip2.ThisBattlePosbilityDaySp4 = (double)Posbility / 150.0;
                    else currentShip2.ThisBattlePosbilityDaySp4 = 0;
                }
                else if (Seiku == BattleSeikuKinds.Yuusei)
                {
                    int Posbility = (int)((double)ShipLuckSpUp + (AllDeckSpUp + ShipSumItemSakuteki[DeckIdx] * 1.2) * 0.6);
                    if (currentShip2.CanDayRenzoku)
                        currentShip2.ThisBattlePosbilityDayRenzoku = (double)Posbility / 130.0;
                    else currentShip2.ThisBattlePosbilityDayRenzoku = 0;
                    if (currentShip2.CanDaySp4)
                        currentShip2.ThisBattlePosbilityDaySp4 = (double)Posbility / 150.0;
                    else currentShip2.ThisBattlePosbilityDaySp4 = 0;
                }
                ++DeckIdx;
            }


        }
        private void CalculateDayBaseHoug()//各种乘数补正前
        {
            foreach (ShipData currentShip in fDeck.Ship_List)
            {
                int slotHoug = 0;//本项V版代码中和战斗无关，直接加到船上
                double slotPlus = 0;
                foreach (ItemData currentItem in currentShip.Item_List)
                {
                    slotHoug += currentItem.Houg;
                    slotPlus += getHougSlotPlus_Attack(currentItem);
                }
                currentShip.ThisBattleBaseHoug = FightParams.valance1 + (double)(currentShip.Houg + slotHoug) + slotPlus;
            }
        }
        private double getHougSlotPlus_Attack(ItemData mstItem)
        {
            double result = 0.0;
            if (mstItem.slotLevel <= 0)
            {
                return result;
            }
            if (mstItem.Api_mapbattle_type3 == 5 || mstItem.Api_mapbattle_type3 == 22)
            {
                return result;
            }
            double num = 2.0;
            if (mstItem.Houg > 12)
            {
                num = 3.0;
            }
            if (mstItem.Api_mapbattle_type3 == 12 || mstItem.Api_mapbattle_type3 == 13 || mstItem.Api_mapbattle_type3 == 16 || mstItem.Api_mapbattle_type3 == 17 || mstItem.Api_mapbattle_type3 == 27 || mstItem.Api_mapbattle_type3 == 28)
            {
                num = 0.0;
            }
            else if (mstItem.Api_mapbattle_type3 == 14 || mstItem.Api_mapbattle_type3 == 15 || mstItem.Api_mapbattle_type3 == 40)
            {
                num = 1.5;
            }
            return num * Math.Sqrt((double)mstItem.slotLevel) * 0.5;
        }
        private void CalculateDayBaseHit()//各种乘数补正前
        {
            foreach (ShipData currentShip in fDeck.Ship_List)
            {
                int slotHit = 0;
                double slotPlus = 0;
                foreach (ItemData currentItem in currentShip.Item_List)
                {
                    slotHit += currentItem.Houm;
                    slotPlus += getHougSlotPlus_Hit(currentItem);
                }
                double middle = Math.Sqrt((double)currentShip.Luck * 1.5) + Math.Sqrt((double)currentShip.Level) * 2.0 + (double)slotHit;
                currentShip.ThisBattleBaseHit = FightParams.valance2 + middle + slotPlus;
            }
        }
        double getHougSlotPlus_Hit(ItemData mstItem)
        {
            double result = 0.0;
            if (mstItem.slotLevel <= 0)
            {
                return result;
            }
            if (mstItem.Api_mapbattle_type3 == 5 || mstItem.Api_mapbattle_type3 == 22)
            {
                return result;
            }
            if ((mstItem.Api_mapbattle_type3 == 12 || mstItem.Api_mapbattle_type3 == 13) && mstItem.Houm > 2)
            {
                result = Math.Sqrt((double)mstItem.slotLevel) * 1.7;
            }
            else if (mstItem.Api_mapbattle_type3 == 21 || mstItem.Api_mapbattle_type3 == 14 || mstItem.Api_mapbattle_type3 == 40 || mstItem.Api_mapbattle_type3 == 16 || mstItem.Api_mapbattle_type3 == 27 || mstItem.Api_mapbattle_type3 == 28 || mstItem.Api_mapbattle_type3 == 17 || mstItem.Api_mapbattle_type3 == 15)
            {
                return result;//这里
            }
            return result = Math.Sqrt((double)mstItem.slotLevel);//和这里交换了一下，更改了V版代码
        }
        //单个表中不存在重复的元素，表是有序的,同值项概率相加
        LinkedList<PosShipDamage> ReduceSortedListsByPlus(LinkedList<PosShipDamage> First_List, LinkedList<PosShipDamage> Second_List)
        {
            //单个表中不存在重复的元素，表是有序的                      
            LinkedList<PosShipDamage> NewList = new LinkedList<PosShipDamage>();
            LinkedListNode<PosShipDamage> currentFirstNode = First_List.First;
            LinkedListNode<PosShipDamage> currentSecondNode = Second_List.First;
            while (currentFirstNode.Next == null && currentSecondNode.Next == null)
            {
                if (currentSecondNode.Next == null || currentFirstNode.Value.Hp < currentSecondNode.Value.Hp)
                {
                    PosShipDamage Item = new PosShipDamage();
                    Item.Hp = currentFirstNode.Value.Hp;
                    Item.Posbility = currentFirstNode.Value.Posbility;
                    NewList.AddLast(Item);
                    currentFirstNode = currentFirstNode.Next;
                    continue;
                }
                if (currentFirstNode.Next == null || currentFirstNode.Value.Hp > currentSecondNode.Value.Hp)
                {
                    PosShipDamage Item = new PosShipDamage();
                    Item.Hp = currentSecondNode.Value.Hp;
                    Item.Posbility = currentSecondNode.Value.Posbility;
                    NewList.AddLast(Item);
                    currentSecondNode = currentSecondNode.Next;
                    continue;
                }
                if (currentFirstNode.Value.Hp == currentSecondNode.Value.Hp)
                {
                    PosShipDamage Item = new PosShipDamage();
                    Item.Hp = currentFirstNode.Value.Hp;
                    Item.Posbility = currentFirstNode.Value.Posbility + currentSecondNode.Value.Posbility;
                    NewList.AddLast(Item);
                    currentFirstNode = currentFirstNode.Next;
                    currentSecondNode = currentSecondNode.Next;
                }
            }
            return NewList;
        }
        private void DeckDirection()
        {
            List<List<PosShipDamage>> FirstPosibilityEdeckDamaged = new List<List<PosShipDamage>>();
            List<List<PosShipDamage>> NextPosibilityEdeckDamaged = new List<List<PosShipDamage>>();
            bool ispossibleT_enemy = false;
            foreach (ShipData CurrentShip in eDeck.Ship_List)
            {
                FirstPosibilityEdeckDamaged.Add(CurrentShip.PosibilityDamaged_List.ToList());
            }
            //相互加合
            NextPosibilityEdeckDamaged = HougekiAttck(0.15,FirstPosibilityEdeckDamaged);//有利           
            NextPosibility = TakeTarget(0.45);//同行               
            NextPosibility = TakeTarget(0.40);//返航(带彩云）
        }
         HougekiAttack(double startPosibility,List<List<PosShipDamage>> eDeckPosDamaged)//友军按顺序攻击
        {
            //好像在这里再循环一次比较好？对方状态在何时判定？

            foreach (ShipData CurrentShip in fDeck.Ship_List)
                TakeTarget(CurrentShip,eDeckPosDamaged);
            return
        }
        List<List<PosShipDamage>> TakeAttaker(ShipData Attaker,List<List<PosShipDamage>> eDeckPosDamaged)
        {
            //假设我方船完全相同，且都可以炮击
            


            return HougekiBattle(nowPosibility);
        }
        
        List<PosShipDamage> TakeTarget(ShipData FAttacker)
        {
            //前船的战斗会影响此处的缘护率
            int numShipInDeck = eDeck.Ship_List.Count;
            List<double> PosNormal= new List<double>(); //旗舰外Normal率
            foreach(ShipData currentShip in eDeck.Ship_List.Skip(1))
            {
                PosNormal.Add(currentShip.StateDamaged_dictonary["Normal"]);
            }
            //每艘船进行缘护的概率，阵型缘护发生率*自身Normal率*1/（非旗舰船Normal数概率+1）
            //缘护不发生时被选中的概率
            //最后合并成每艘船被选择的概率
            //和每艘船的受伤状态有关，Normal，击沉，影响到后续伤害
            
            }

    //概率函数for（i,j） 有序增长
    KindAttack(ShipData Fship, ShipData Eship)
        {

        }
        
    }
}