import { Pipe, PipeTransform } from '@angular/core';
import { min } from 'rxjs';

@Pipe({
  name: 'timeAgo',
})
export class TimeAgoPipe implements PipeTransform {
  transform(value: string): string {
    const seconds = Math.floor((Date.now() - new Date(value).getTime()) / 1000);

    if(seconds < 60) return "just now";
    const minutes = Math.floor(seconds / 60);
    if(minutes < 60) return `${minutes} minute${this._getPlular(minutes)} ago`
    const hours = Math.floor(minutes/60);
    if(hours < 24) return `${hours} hour${this._getPlular(hours)} ago`;
    const days = Math.floor(hours/24);
    return `${days} day${this._getPlular(days)} ago`
  
  }
  _getPlular(value:any){
    return value > 1 ? "s" : "";
  }
}
