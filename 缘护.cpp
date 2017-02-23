#include<iostream>
#include<cmath>
using namespace std;
double WO1Attack(int numE, int numF, double rate);
//double WO2Attack(int numE, int numF, double rate);
int main()
{
	int num = 6;
	while (num > 0)
	{
		double rate = WO1Attack(num, 4, 1);
		std::cout << num << " " << rate << endl;
		--num;
	}
	/*while (num > 0)
	{
	double rate = WO2Attack(num, 4, 1);
	std::cout << num << " " << rate << endl;
	--num;
	}*/
	return 0;
}
double WO1Attack(int numE, int numF, double rate)
{
	if (numF > numE)//我方出手数不会多余地方舰数
		numF = numE;
	if (numF <= 0)//我方最后一舰出手
		return 0;
	double DnumE = numE;
	double result = 0;
	//我方已出手
	double rateSelectWO = 1.0 / DnumE*(1.0 - 0.75*(DnumE - 1.0) / DnumE);//本次选中wo改
	double rateWOKill = rate*rateSelectWO;//本次选中wo改并*0.45命中，击杀wo改
	result += rateWOKill;
	//本次未选中wo改
	double rateUNkillE = rate*0.22;//未命中僚舰	
	result += WO1Attack(numE, numF - 1, rateUNkillE);//进入下一轮炮击
	double rateKillE = rate*0.78;//命中僚舰													  //敌方被消灭
	result += WO1Attack(numE - 1, numF - 1, rateKillE);
	return result;

}
//double WO1Attack(int numE, int numF, double rate)
//{
//	double DnumE = numE;
//	double result = 0;
//	double rateWOhit00 = 1.0 / DnumE*(1.0 - 0.45*(DnumE - 1.0) / DnumE)*0.45;
//	rateWOhit00 *= rate;
//	double rateWOhit01 = (1.0 - rateWOhit00) / DnumE*0.45;
//	rateWOhit01 *= rate;
//	result+= 
//	
//
//}