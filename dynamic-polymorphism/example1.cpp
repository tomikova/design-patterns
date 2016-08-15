#include <cstdlib>
#include <iostream>

using namespace std;

class CoolClass{
  public:
    virtual void set(int x){x_=x;};
    virtual int get(){return x_;};
  private:
    int x_;
  };
  
  class PlainOldClass{
  public:
    void set(int x){x_=x;};
    int get(){return x_;};
  private:
    int x_;
  };


int main(int argc, char *argv[])
{
    
    printf("sizeof(CoolClass) = %d\n", sizeof(CoolClass));
    printf("sizeof(PlainOldClass) = %d\n", sizeof(PlainOldClass));
    
    system("PAUSE");
    return EXIT_SUCCESS;
}
