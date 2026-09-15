import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-google-callback',
  imports: [],
  templateUrl: './google-callback.html',
  styleUrl: './google-callback.scss',
})
export class GoogleCallback implements OnInit{
  ngOnInit(): void {
    const hashParams = new URLSearchParams(window.location.hash.substring(1));
    const idToken = hashParams.get('id_token');

    if (idToken) {
      window.opener?.postMessage({ type: 'google-auth', credential: idToken }, window.location.origin);
    }
    window.close();
  }

}
