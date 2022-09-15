import Oidc from 'oidc-client';

const config = {
  authority: "http://localhost:5000",
  client_id: "js",
  redirect_uri: "http://localhost:8080", 
  //response_type: "id_token token",
  response_type: "code",
  scope:"openid profile vouapi",
  post_logout_redirect_uri : "http://localhost:8080",
}
  
export const mgr = new Oidc.UserManager(config)

export const login = () => mgr.signinRedirect()
  
export const logout = () => mgr.signoutRedirect()