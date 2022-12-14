import {LoginModel} from "../types/loginModel";
import LoginForm from "./LoginForm";
import {useLogin} from "../hooks/UserHooks";


const Login = () => {
    const checkLogin = useLogin();
    const loginModel:LoginModel = {
        username: '',
        password: '',
        rememberMe: false

    };
    return (
        <LoginForm login={loginModel} submitted={(login) => checkLogin.mutate(login)}></LoginForm>
    );

}
export default Login;