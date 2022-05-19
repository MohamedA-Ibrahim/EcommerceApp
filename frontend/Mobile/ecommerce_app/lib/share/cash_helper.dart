
import 'package:shared_preferences/shared_preferences.dart';

class CacheHelper
{
  static late SharedPreferences sharedPreferences;

  static init() async
  {
    sharedPreferences = await SharedPreferences.getInstance();
  }
  static Future<bool> saveToken(String token)
  {
    return sharedPreferences.setString("token", token);
  }
  static String? getToken()
  {
    return sharedPreferences.getString("token");
  }
  static Future<bool> removeToken()
  {
    return sharedPreferences.remove("token");
  }
  static Future<bool> saveRefreshToken(String refreshToken)
  {
    return sharedPreferences.setString("refreshToken", refreshToken);
  }
  static String? getRefreshToken()
  {
    return sharedPreferences.getString("refreshToken");
  }
  static Future<bool> removeRefreshToken()
  {
    return sharedPreferences.remove("refreshToken");
  }
  static Future<bool> setAdmin(bool value)
  {
    return sharedPreferences.setBool("isAdmin", value);
  }
  static bool? isAdmin()
  {
    return sharedPreferences.getBool("isAdmin");
  }
  static Future<bool> removeIsAdmin()
  {
    return sharedPreferences.remove("isAdmin");
  }
}