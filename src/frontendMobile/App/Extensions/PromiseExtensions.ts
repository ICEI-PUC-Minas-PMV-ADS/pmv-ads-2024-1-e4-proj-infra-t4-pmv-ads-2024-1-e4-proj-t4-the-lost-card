const timedReject = <TTimeout,>(millis: number) => new Promise<TTimeout>((resolve, reject) =>
    setTimeout(() => reject(`Timed out after ${millis} ms.`), millis));

export default { timedReject };