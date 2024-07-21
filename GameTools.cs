using System.Reflection.Metadata.Ecma335;

namespace ToolNamespace;

static class Text{
    //Dimensiones del texto
    //(int x,int y,int width,int height) textdim;
    //Borra el texto anterior creado.
    public static void Start(string text,int speed=25){
        var cursorInitialLeft = Console.CursorLeft;
        foreach(char c in text){
            if(c=='\n')
            {
                Console.SetCursorPosition(cursorInitialLeft,Console.CursorTop+1);
            }else{
                Console.Write(c);
            }
            Thread.Sleep(speed);
        }

        //Limpio el buffer, por si se cola alguna entrada.
        while(Console.KeyAvailable){
            Console.ReadKey(true);
        }
    }
    public static void WriteCenter(string text,int width){
        int espacios = (width-text.Length)/2;
        Console.CursorLeft += espacios;
        Console.Write(text);
    }
    public static void borrarSeccion(int x, int y, int width, int height){
      Console.CursorTop = y;
      for(int i = 0; i<=height;i++){
            Console.CursorLeft = x;
            Console.Write(new string(' ', width));
            Console.CursorTop++;     
      }  
    }
}
class Caja{
    int x,y,width,height;
    public int X { get => x;}
    public int Y { get => y;}
    public int Width { get => width;}
    public int Height { get => height;}

    (int Left, int Top) cursorWritter;

    public (int Left, int Top) CursorWritter { get => cursorWritter;}

    public Caja(int x, int y, int width, int height){
        this.x = x;
        this.y = y;
        this.width =width;
        this.height = height; 
        Console.SetCursorPosition(x,y);
        Console.WriteLine("╔"+new string('═',width-2)+"╗");
        for(int i=0; i < height-2;i++)
        {
            Console.CursorLeft = x;
            Console.WriteLine("║" + new string(' ',width-2)+"║");
        }
        Console.CursorLeft = x;
        Console.WriteLine("╚"+new string('═',width-2)+"╝");
        cursorWritter = (Left: x+2,Top:y+1);
    }
    public void EscribirAnim(string text, int xlocal=0,int ylocal=0,int speed=25){
        Console.SetCursorPosition(cursorWritter.Left + xlocal,cursorWritter.Top + ylocal);
        Text.Start(text,speed);
    }
    public void Escribir(string text,int xlocal=0,int ylocal=0){
        string[] sub = text.Split('\n');
        Console.SetCursorPosition(cursorWritter.Left + xlocal,cursorWritter.Top + ylocal);
        foreach(string s in sub){
            Console.Write(s);
            Console.SetCursorPosition(cursorWritter.Left + xlocal,Console.CursorTop+1);
        };
    }
}