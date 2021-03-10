import { HttpEvent, HttpHandler, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthHttpInterceptor } from "@auth0/auth0-angular";
import { Observable } from "rxjs";

@Injectable()
export class AuthHttpExtendedInterceptor extends AuthHttpInterceptor {
    
    private whitelist = [
        { url: '/discipline$', method: 'GET' },
        { url: '/tournament$', method: 'GET' },
        { url: '/tournament\/[0-9]*$', method: 'GET' },
        { url: '\/tournament\/[0-9]*\/bracket$', method: 'GET' }
    ];

    private isWhitelisted(url: string, method: string): boolean {
        let isWhitelisted = false;
        this.whitelist.forEach(element => {
            if (new RegExp(element.url).exec(url)) {
                if (method == element.method) {
                    isWhitelisted = true;
                }
            }
        });

        return isWhitelisted;
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (this.isWhitelisted(req.url, req.method)) {
            return next.handle(req);
        }
        else {
            return super.intercept(req, next);
        }
    }
}