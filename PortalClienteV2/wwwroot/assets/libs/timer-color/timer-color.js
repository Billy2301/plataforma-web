// Credit: Mateusz Rybczonec

var targetDate = new Date(fechaExpiracion).getTime();
//Obtiene la fecha y hora actuales
var now = new Date().getTime();
//    // Calcula la diferencia horaria entre la fecha/hora actual y la fecha/hora objetivo
var timeDifference = targetDate - now;

// Convierte la diferencia de tiempo a segundos
var totalSeconds = Math.floor(timeDifference / 1000);


const FULL_DASH_ARRAY = 283;
const WARNING_THRESHOLD = 180;
const ALERT_THRESHOLD = 60;

const COLOR_CODES = {
  info: {
    color: "green"
  },
  warning: {
    color: "orange",
    threshold: WARNING_THRESHOLD
  },
  alert: {
    color: "red",
    threshold: ALERT_THRESHOLD
  }
};

var TIME_TOTAL = tiempoTotal; // 5 min
var TIME_LIMIT = totalSeconds;
let timePassed = 0;
let timeLeft = totalSeconds;
let timerInterval = null;
let remainingPathColor = COLOR_CODES.info.color;

document.getElementById("app").innerHTML = `
<div class="base-timer">
  <svg  viewBox="0 0 24 24">
    <rect width="2" height="7" x="11" y="6" fill="currentColor" rx="1" id="base-timer-rect-minute" class="base-timer__rec green">
        <animateTransform attributeName="transform" dur="9s" repeatCount="indefinite" type="rotate" values="0 12 12;360 12 12"></animateTransform></rect>
    <rect width="2" height="9" x="11" y="11" fill="currentColor" rx="1" id="base-timer-rect-second" class="base-timer__rec green">
        <animateTransform attributeName="transform" dur="0.75s" repeatCount="indefinite" type="rotate" values="0 12 12;360 12 12"></animateTransform></rect>

  <svg class="base-timer__svg" viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg">
    <g class="base-timer__circle">
      <circle class="base-timer__path-elapsed" cx="50" cy="50" r="45"></circle>
      <path
        id="base-timer-path-remaining"
        stroke-dasharray="283"
        class="base-timer__path-remaining ${remainingPathColor}"
        d="
          M 50, 50
          m -45, 0
          a 45,45 0 1,0 90,0
          a 45,45 0 1,0 -90,0
        "
      ></path>
    </g>
  </svg></svg></div>
  <span id="base-timer-label" class="base-timer__label">${formatTime(
    timeLeft
  )}</span>

`;

/*startTimer();*/

function onTimesUp() {
    endTimer()
  clearInterval(timerInterval);
}

function resetTimerVariables() {
    TIME_LIMIT = tiempoTotal-1;
    timeLeft = tiempoTotal-1;
}
function startTimer() {
    setTimeout(function () {
        // Agrega la clase "show" después de 3 segundos (3000 ms)
        document.getElementById("app").classList.add("show");
    }, 1000); // 3000 ms = 3 segundos
  
  timerInterval = setInterval(() => {
    timePassed = timePassed += 1;
    timeLeft = TIME_LIMIT - timePassed;
      document.getElementById("base-timer-label").innerHTML = formatTime(timeLeft) + "<small class='base-timer-text'>Para realizar su pago</small>";
    setCircleDasharray();
    setRemainingPathColor(timeLeft);

    if (timeLeft === 0) {
      onTimesUp();
    }
  }, 1000);

    setTimeout(function () {
        // Agrega la clase "show" después de 3 segundos (3000 ms)
        document.getElementById("app").classList.add("mini");
    }, 4000);

}

function formatTime(time) {
    const minutes = Math.floor(time / 60);
    let seconds = time % 60;

    if (seconds < 10) {
        seconds = `0${seconds}`;
    }

    return `${minutes}:${seconds}`;
}

function setRemainingPathColor(timeLeft) {
    const { alert, warning, info } = COLOR_CODES;
    if (timeLeft <= alert.threshold) {
        document.getElementById("base-timer-path-remaining").classList.remove(warning.color);
        document.getElementById("base-timer-path-remaining").classList.add(alert.color);
        document.getElementById("base-timer-rect-minute").classList.remove(warning.color);
        document.getElementById("base-timer-rect-minute").classList.add(alert.color);
        document.getElementById("base-timer-rect-second").classList.remove(warning.color);
        document.getElementById("base-timer-rect-second").classList.add(alert.color);
    } else if (timeLeft <= warning.threshold) {
        document.getElementById("base-timer-path-remaining").classList.remove(info.color);
        document.getElementById("base-timer-path-remaining").classList.add(warning.color);
        document.getElementById("base-timer-rect-minute").classList.remove(info.color);
        document.getElementById("base-timer-rect-minute").classList.add(warning.color);
        document.getElementById("base-timer-rect-second").classList.remove(info.color);
        document.getElementById("base-timer-rect-second").classList.add(warning.color);
    }
}

function calculateTimeFraction() {
    const rawTimeFraction = timeLeft / TIME_TOTAL;
    return rawTimeFraction - (1 / TIME_TOTAL) * (1 - rawTimeFraction);
}

function setCircleDasharray() {
    const circleDasharray = `${(
        calculateTimeFraction() * FULL_DASH_ARRAY
    ).toFixed(0)} 283`;
    document
        .getElementById("base-timer-path-remaining")
        .setAttribute("stroke-dasharray", circleDasharray);
}


