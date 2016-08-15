#include <iostream>

using namespace std;

class AbstractDatabase{
    public:
        virtual int getData()=0; 
};

class MockDatabase: public AbstractDatabase{
    public:
        virtual int getData(){
            return 5;
        }
};

class ConcreteDatabase{
    public:
        virtual int getData(){
            return 6;
        }
};

class Client1 {
    ConcreteDatabase myDatabase;
    public :
        int transaction() {
            myDatabase.getData();
        }
};

class Client2 { 
    AbstractDatabase & myDatabase;
    public :
        Client2 ( AbstractDatabase & db ): myDatabase(db) {}
    public :
        int transaction (){
            myDatabase.getData(); 
        }
};

int main(int argc, char *argv[])
{

    MockDatabase* pdb = new MockDatabase();
    Client2 client (*pdb);
    int b;
    b=client.transaction();
    cout<<b<<endl;
    Client1 clientbio;  
    b=clientbio.transaction();
    cout<<b<<endl;
    
    system("PAUSE");
    return EXIT_SUCCESS;
}
