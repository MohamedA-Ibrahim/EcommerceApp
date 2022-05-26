class Log
{
  static void v(String msg)
  {
    print("////////////////////{--Message--}//////////////////////-->> $msg \n");
  }

  static void catchE(e)
  {
    print("////////////////////{--Catch Error--}//////////////////////-->> \n");
    print("////////////////////{--Error--}//////////////////////-->> ${e.toString()} \n");
  }

  static void e(String e)
  {
    print("////////////////////{--Error--}//////////////////////-->> $e \n");
  }

  static void w(String w)
  {
    print("////////////////////{--Warning--}//////////////////////-->> $w");
  }
  static void faildResponse(value, String type)
  {
    w("Faild response $type");
    w(value.statusCode);
    w(value.data.toString());
  }
}
