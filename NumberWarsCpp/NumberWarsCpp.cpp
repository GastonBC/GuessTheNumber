#include <iostream>
#include <vector>
#include <random>
#include <string>
#include <iterator>
#include <algorithm>
#include "Utils.h"

using namespace std;

std::string GiveNum(int digits)
{
    std::string in = "0123456789", out;

    std::sample(in.begin(), in.end(), std::back_inserter(out), digits, std::mt19937{ std::random_device{}() });

    return out;
}

int main()
{

    int GAME_NUMBER = 10000;
    int TOTAL_STEPS = 0;
    int DIGITS = 4;

    


    std::string Code = GiveNum(4);
    std::string Guess = "0123";

    //std::vector<int> Result = Utils.GuessResult(Code, Guess);


    std::cout << Code << "\n";
    std::cout << Guess << "\n";
    //std::cout << Result[0] << Result[1] << "\n";

    return 0;
}