import { useEffect, useState } from 'react';
import { fileSystem } from '../services/agent';

const Body: React.FC = () => {
  const [files$] = useState(() => fileSystem.listFiles());
  const [files, setFiles] = useState<string[]>([]);
  const [loaded, setLoaded] = useState(false);

  useEffect(() => {
    if (loaded) return;

    const subscription = files$.subscribe({
      next: (x) => {
        setFiles((f) => [...f, x.getPath()]);
      },
      complete: () => setLoaded(true),
    });

    return () => {
      console.log('unsubscribing');
      subscription.unsubscribe();
    };
  }, [files$, loaded]);

  return (
    <div>
      <h4>Body</h4>
      {files.map((file) => (
        <div key={file}>
          <span>File: {file}</span>
        </div>
      ))}
    </div>
  );
};

export default Body;
