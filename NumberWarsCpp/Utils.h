#include <iostream>
#include <vector>
#include <random>
#include <string>
#include <iterator>
#include <algorithm>
#ifndef Utils_H
#define Utils_H
#endif // !Utils_H

using namespace std;

std::string GiveNum(int digits);

std::vector<int> GuessResult(std::string Code, std::string Guess, int& GoodAmmount, int& RegularAmmount);
