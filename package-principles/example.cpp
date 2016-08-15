# include "Client.h"
# include "Derived3.h"
# include <iostream>

using namespace std;

int main(int argc, char *argv[])
{
    Derived3 d;
    Client c(d);
    c.operate();
    
  system("PAUSE");	
  return 0;
}
