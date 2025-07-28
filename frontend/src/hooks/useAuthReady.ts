import { useEffect, useState } from 'react';

const useAuthReady = () => {
  const [ready, setReady] = useState(false);

  useEffect(() => {
    const init = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        try {
          const res = await fetch('http://localhost:56398/api/Auth/refresh', {
            method: 'POST',
            credentials: 'include',
          });
          if (res.ok) {
            const data = await res.json();
            localStorage.setItem('token', data.token);
          }
        } catch {
          localStorage.removeItem('token');
        }
      }
      setReady(true);
    };

    init();
  }, []);

  return ready;
};

export default useAuthReady;
