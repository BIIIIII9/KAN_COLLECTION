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
	if (numF > numE)//�ҷ��������������ط�����
		numF = numE;
	if (numF <= 0)//�ҷ����һ������
		return 0;
	double DnumE = numE;
	double result = 0;
	//�ҷ��ѳ���
	double rateSelectWO = 1.0 / DnumE*(1.0 - 0.75*(DnumE - 1.0) / DnumE);//����ѡ��wo��
	double rateWOKill = rate*rateSelectWO;//����ѡ��wo�Ĳ�*0.45���У���ɱwo��
	result += rateWOKill;
	//����δѡ��wo��
	double rateUNkillE = rate*0.22;//δ�����Ž�	
	result += WO1Attack(numE, numF - 1, rateUNkillE);//������һ���ڻ�
	double rateKillE = rate*0.78;//�����Ž�													  //�з�������
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