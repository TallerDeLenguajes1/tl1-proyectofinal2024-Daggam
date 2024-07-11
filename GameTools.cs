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
    }
    public static void WriteCenter(string text){
        int espacios = (Console.WindowWidth-text.Length)/2;
        Console.CursorLeft = espacios;
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
    (int Left, int Top) cursorWritter;

    public Caja(int x, int y, int width, int height){
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
    public void Escribir(string text, int speed=25){
        Console.SetCursorPosition(cursorWritter.Left,cursorWritter.Top);
        Text.Start(text,speed);
    }
}