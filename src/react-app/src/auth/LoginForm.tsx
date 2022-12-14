import {LoginModel} from "../types/loginModel";
import {useState} from "react";

type Args = {
    login: LoginModel
    submitted:(login:LoginModel) => void
}

const LoginForm = ({login, submitted}:Args) => {
    const [loginState, setLoginState] = useState({...login});

    const onSubmit: React.MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault();
        submitted(loginState);
    };


    return (
        <div className="row">
            <div className="col-md-6 offset-md-3">
                <h2>Login</h2>
                <form name="form" >
                    <div className="form-group">
                        <label>Username</label>
                        <input type="text" name="username" value={loginState.username} onChange={(e) =>setLoginState({...loginState,username:e.target.value})} className={'form-control'}/>
                    </div>
                    <div className="form-group">
                        <label>Password</label>
                        <input type="password" name="password" value={loginState.password} onChange={(e) => setLoginState({...loginState,password:e.target.value}) } className={'form-control'}/>
                    </div>
                    <div className="form-group">
                        <label className={'form-check-label'}>Remember Me</label>

                        <input type="checkbox" name="rememberMe" checked={loginState.rememberMe} onChange={(e) => setLoginState({...loginState,rememberMe: e.target.checked}) } className={'form-check-input'}/>
                    </div>
                    <div className="form-group">
                        <button className="btn btn-primary" disabled={!loginState.username || !loginState.password} onClick={onSubmit}>Login</button>
                    </div>
                </form>
            </div>
        </div>
    );
};
    export default LoginForm;

