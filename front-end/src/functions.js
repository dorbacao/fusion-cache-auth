import toastr from 'toastr';
import 'toastr/build/toastr.min.css';

function run(session, url, timeLabel, progressBar, totalRequests, completedRequests, token, sessionId, errorLabel){
    const requests = [];
    let start = Date.now();
    let succesCount = 0;
    let errorCount = 0;
    const errorLbl = document.getElementById(errorLabel);
    
    const updateProgressBar = () => {
        const progress = (completedRequests * 100) / totalRequests;
        progressBar.style.width = progress + '%';
        progressBar.textContent = progress + '%';
    };

    progressBar.className = '';            
    progressBar.classList.add('progress-bar');
    errorLbl.textContent = '';
    progressBar.textContent =  '0%';


    updateProgressBar();

    for (let i = 0; i < totalRequests; i++) {
        var head = {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        };
        
        if(session){
            head = {
                'Content-Type': 'application/json',
                'session-id': sessionId
            };
        }

        requests.push(fetch(url, {
            method: 'GET',
            headers: head
        }).then(response => {
            if (response.ok) {
                completedRequests++;
                succesCount++;
                updateProgressBar();
            }
        }).catch(error => {
            completedRequests++;
            errorCount++;
            updateProgressBar();
            progressBar.className = '';            
            progressBar.classList.add('progress-bar-error');
        }));
    } 
    
    Promise.all(requests)
    .then(() => { 
        toastr.info('Todas as requisçoes terminaram');
        let end = Date.now();
        const diff = end - start;
        const seconds = diff / 1000;
        timeLabel.textContent = seconds + 's';

        let msg = `${succesCount} success / ${errorCount} error`;
        errorLbl.textContent = msg;
    });

   

}
function updateProgressBar(session, progressBarId, timeLabelId, errorLabelId) {

    const totalRequests = document.getElementById('totalRequests').value;
    
    
    const token = localStorage.getItem('token');
    const sessionId = localStorage.getItem('session-id');
    const url = 'http://localhost:5204/user/name';
    const progressBar = document.getElementById(progressBarId);
    const timeLabel = document.getElementById(timeLabelId);
    let completedRequests = 0;


    progressBar.style.width = '0%';

    setTimeout(()=>run(session, url, timeLabel, progressBar, totalRequests, completedRequests, token, sessionId, errorLabelId), 2000);

}


export function progressBar1(element, element2) {
    element.addEventListener('click', () => updateProgressBar(false,'progressBar1', 'timeLabel1', 'errorLabel1'))
    element2.addEventListener('click', () => updateProgressBar(true,'progressBar2', 'timeLabel2', 'errorLabel2'))
}
