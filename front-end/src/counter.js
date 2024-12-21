import toastr from 'toastr';
import 'toastr/build/toastr.min.css';


export function setupCounter(element) {
  let counter = 0
  const login = () => {
  
    const url = 'http://localhost:5204/user/login'; 
    const data = { Login: 'vinicius', 
      Password: '123' }; 

    fetch(url, 
      { 
        method: 'POST', headers: { 'Content-Type': 'application/json' }, 
        body: JSON.stringify(data)
    }).then(response => response.json())
    .then(data => {
      localStorage.setItem('token', data.accessToken);
      localStorage.setItem('session-id', data.sessionId);
      toastr.success('Login efetuado com sucesso', 'Sucesso');
      setTimeout('window.location = "teste.html";',2000);
      
    })
    .catch(error =>{
      toastr.error('Erro ao efetuar o login', 'Erro');
    }
    );
    
  }
  element.addEventListener('click', () => login())
  setCounter(0)
}
