#include <iostream>
#include <vector>
#include <random>
#include <string>
#include <iterator>
#include <algorithm>

using namespace std;

#pragma once
class Utils
{
public:
	static std::string GiveNum(int digits);

	static std::vector<int> GuessResult(std::string Code, std::string Guess, int& GoodAmmount, int& RegularAmmount);

private:
	Utils() {}
};