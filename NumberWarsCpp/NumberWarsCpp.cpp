#include <iostream>
#include <vector>
#include <random>
#include <string>
#include <iterator>
#include <algorithm>
#include "Utils.h"
#include <set>

using namespace std;


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