#include <cstdlib>
#include <iostream>

using namespace std;

class Base{
public:
  Base() {
    method();
  }

  virtual void virtualMethod() {
    printf("Im base implementation!\n");
  }

  void method() {
    printf("Method says: ");
    virtualMethod();
  }
};

class Derived: public Base{
public:
  Derived(): Base() {
    method();
  }
  virtual void virtualMethod() {
    printf("Im derived implementation!\n");
  }
};

int main(int argc, char *argv[])
{    
    Derived* pd=new Derived();
    pd->method();
    
    system("PAUSE");
    return EXIT_SUCCESS;
}


