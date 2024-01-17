import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import {NavigationExtras, Router} from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorsInterceptor implements HttpInterceptor {

  constructor(private router:Router,private toastr:ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse)=>
{
  if(error){
    switch(error.status){
      case 400:
        //error the first in the switch the second is the object the third is what is coming back from API services
        if(error.error.errors){
        const modelStateErrors = [];
      for (const key in error.error.errors){
        if(error.error.errors[key])
        {
        modelStateErrors.push(error.error.errors[key])
        }
      } throw modelStateErrors.flat();
    }else{
      this.toastr.error(error.error,error.status.toString())
    }
    break;
   case 401:
    this.toastr.error('Unautherized',error.status.toString())
    break;
    case 404:
      this.router.navigateByUrl('/not-found');
      break;
      case 500:
        //here the error in a back page
        const navigationExtras:NavigationExtras = {state: {error: error.error}}
        this.router.navigateByUrl('/server-error',navigationExtras);
        break;
        default:
          this.toastr.error('something unexpected happened!');
          console.log(error);
          break;
    }

  } throw error;
}

      )
    )
  }
}
