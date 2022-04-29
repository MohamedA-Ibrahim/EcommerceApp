class UserModel
{
  String? token;
  String? refreshToken;

  UserModel(this.token, this.refreshToken);
  UserModel.fromJson(Map<String, dynamic> json)
  {
    token = json["token"];
    refreshToken = json["refreshToken"];
  }
}