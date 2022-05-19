class Log
{
  static void v(String msg)
  {
    print("////////////////////{--Message--}//////////////////////-->> $msg");
  }

  static void catchE(e)
  {
    print("////////////////////{--Catch Error--}//////////////////////-->>");
    print("////////////////////{--Error--}//////////////////////-->> ${e.toString()}");
  }

  static void e(String e)
  {
    print("////////////////////{--Error--}//////////////////////-->> $e");
  }

  static void w(String w)
  {
    print("////////////////////{--Warning--}//////////////////////-->> $w");
  }
}
