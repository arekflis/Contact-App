import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Footer } from "./components/footer/footer";
import { Header } from "./components/header/header";
import { Nav } from "./components/nav/nav";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Footer, Header, Nav],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'contact-app-frontend';
}
