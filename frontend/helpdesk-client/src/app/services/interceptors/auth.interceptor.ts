import { HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { AuthService } from "../auth.service";

export const authInterceptor: HttpInterceptorFn = (req,next) =>{
    const authService = inject(AuthService);
    const token = authService.getToken();

    const isApiRequest = req.url.startsWith('http://localhost:5029/api/');
    const isAuthRequest = req.url.endsWith('/auth/login') || req.url.endsWith('/auth/register');

    if(token && isApiRequest && !isAuthRequest)
    {
        const authReq = req.clone ({
            setHeaders:{
                Authorization : `bearer ${token}`
            }
        });

        return next(authReq);
    }
    return next(req);
}