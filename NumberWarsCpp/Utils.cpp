#include <iostream>
#include <vector>
#include <random>
#include <string>
#include <iterator>
#include <algorithm>

using namespace std;


std::string GiveNum(int digits)
{
    std::string in = "0123456789", out;

    std::sample(in.begin(), in.end(), std::back_inserter(out), digits, std::mt19937{ std::random_device{}() });

    return out;
}

std::vector<int> GuessResult(std::string Code, std::string Guess, int& GoodAmmount, int& RegularAmmount)
{
    // First check good numbers. Check player index num against same index on npc
    for (int i = 0; i < Guess.length(); i++)
    {
        if (Guess[i] == Code[i])
        {
            GoodAmmount++;
        }
    }

    // Then check regular numbers. Check each index agains all indexes of NPC_NUM
    for (int i = 0; i < Guess.length(); i++)
    {
        for (int n = 0; n < Code.length(); n++)
        {
            // If it's a different index (that'd be a good number) and it's the
            // same number then increment a regular
            if (i != n && Guess[i] == Code[n])
            {
                RegularAmmount++;
            }

        }
    }

    std::vector<int> OutList = { GoodAmmount, RegularAmmount };

    return OutList;
}
