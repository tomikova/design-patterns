#include <stdio.h>
#include <stdlib.h>

typedef char const* (*PTRFUN)();
  
struct Animal 
{
     char *name;
     PTRFUN *AnimalTable;
};

typedef struct Animal Animal;

char const* DogGreet()
{
     return "vau!";       
}

char const* CatGreet()
{ 
     return "meow!";
}

char const* DogMenu()
{
     return "beef";
}

char const* CatMenu()
{
     return "tuna";
}

PTRFUN DogTable[2] = { &DogGreet, &DogMenu };
PTRFUN CatTable[2] = { &CatGreet, &CatMenu };

Animal* createDog (char *name)
{
      Animal* Dog = (Animal *)malloc(sizeof(Animal));
     
      Dog->name = name;
      Dog->AnimalTable = DogTable;
     
     return Dog;
}

Animal* createCat(char *name)
{
       Animal* Cat =(Animal *)malloc(sizeof(Animal));
      
       Cat->name = name;
       Cat->AnimalTable = CatTable;
      
      return Cat;
}

void animalPrintGreeting(Animal* animal)
{
     printf("%s pozdravlja %s \n",animal->name,animal->AnimalTable[0]());
}

void animalPrintMenu(Animal *animal)
{
     printf("%s voli %s \n",animal->name,animal->AnimalTable[1]());
}

void TestAnimal()
{
   
      Animal* p1=createDog("Hamlet");
      Animal* p2=createCat("Ofelija");
      Animal* p3=createDog("Polonije");

      animalPrintGreeting(p1);
      animalPrintGreeting(p2);
      animalPrintGreeting(p3);

      animalPrintMenu(p1);
      animalPrintMenu(p2);
      animalPrintMenu(p3);
      
      free(p1); free(p2); free(p3);
}

int main(int argc, char *argv[])
{
  
  TestAnimal();
  
  system("PAUSE");	
  return 0;
}
