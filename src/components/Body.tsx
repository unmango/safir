import { useEffect, useState } from 'react';
import { list } from '../services/agentClient';

const Body: React.FC = () => {
  const [files, setFiles] = useState<string[]>([]);

  list().subscribe((x) => console.log(x));

  useEffect(() => {
    console.log('in effect');
    const subscription = list().subscribe(
      (x) => {
        console.log('in next');
        setFiles((f) => [...f, x]);
      },
      (e) => console.error(e),
      () => console.log('completed')
    );
    return () => {
      console.log('unsubscribing');
      subscription.unsubscribe();
    };
  }, []);

  return (
    <div>
      <span>Body</span>
      {files.map((file) => (
        <span>File: {file}</span>
      ))}
    </div>
  );
};

export default Body;
