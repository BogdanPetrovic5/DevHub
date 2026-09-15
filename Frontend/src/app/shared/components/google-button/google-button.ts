import { AfterViewInit, Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-google-button',
  imports: [],
  templateUrl: './google-button.html',
  styleUrl: './google-button.scss',
})
export class GoogleButton{
  @Output() credentials = new EventEmitter<{idToken: string, nonce: string | null}>();
  private popupWindow: Window | null = null;
  private messageListener = (event: MessageEvent) => {
    if (event.origin !== window.location.origin) return;
    if (event.data.type !== 'google-auth') return;
    
    const exceptedNonce = sessionStorage.getItem('google_nonce');
    sessionStorage.removeItem('google_nonce');

    this.credentials.emit({
      idToken: event.data.credential,
      nonce: exceptedNonce
    })
    window.removeEventListener('message', this.messageListener);
    this.popupWindow?.close();
  };
  openPopUp() {
    const nonce = crypto.randomUUID();
    sessionStorage.setItem('google_nonce', nonce);

    const params = new URLSearchParams({
      client_id:environment.googleClientId,
      redirect_uri: `${window.location.origin}/auth/google/callback`,
      response_type:'id_token',
      scope:'openid email profile',
      nonce,
      response_mode: 'fragment'
    });
    this.popupWindow = window.open(
      `https://accounts.google.com/o/oauth2/v2/auth?${params}`,
      'GoogleAuth',
      'width=500,height=600'
    );
    window.addEventListener('message', this.messageListener);
  }
  
}
