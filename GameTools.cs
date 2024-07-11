namespace ToolNamespace;

static class Text{
    //Dimensiones del texto
    //(int x,int y,int width,int height) textdim;
    //Borra el texto anterior creado.
    public static void Start(string text,int speed=1000){
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
        Console.Write(new string(' ',espacios) + text);
    }
    public static void borrarSeccion(int x, int y, int width, int height){
      Console.CursorLeft = x;
      Console.CursorTop = y;
      for(int i = 0; i<=height;i++){
            Console.Write(new string(' ', width));
      }  
    }
}
class Box{

}