import './style.css'
import javascriptLogo from './javascript.svg'
import viteLogo from '/vite.svg'
import { progressBar1 } from './functions.js'
import toastr from 'toastr';
import 'toastr/build/toastr.min.css';

// Configuração básica do toastr
toastr.options = {
    "closeButton": true,
    "timeOut": 2000,
    "extendedTimeOut": 1000,
    "positionClass": "toast-top-right"
};

document.querySelector('#app').innerHTML = `
   <div class="container">
        <div class="progress">
            <div class="progress-bar" id="progressBar1"></div>
        </div>
        <div class="time-label" id="timeLabel1">0.000s</div>
        
    </div>
    <div class="container">
      <div class="time-label" id="errorLabel1"></div>
    </div>

    <div class="container">
        <div class="progress">
            <div class="progress-bar" id="progressBar2"></div>
        </div>
        <div class="time-label" id="timeLabel2">0.000s</div>
    </div>
     <div class="container">
      <div class="time-label" id="errorLabel2"></div>
     </div>
     <div class="card">
      <div class="container">
      <label for="totalRequests" class="label">Total Requisições:</label>
      <input type="number" id="totalRequests" class="input" value="200" placeholder="Digite o número total">
      </div>
    </div>
     <div class="card">
      <button id="bearerTesteButton" type="button">Testar Bearer Token</button>
       <button id="sessionIdButton" type="button">Testar Session Id</button>
    </div>
    
`

progressBar1(document.querySelector('#bearerTesteButton'), document.querySelector('#sessionIdButton'))
