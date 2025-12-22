using Project3.Application.Dtos.Background;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Queues
{
    public class ExamSubmitQueue
    {
        private readonly object _lock = new();

        private readonly PriorityQueue<ExamSubmitJob, DateTime> _queue = new();
        private readonly HashSet<string> _keys = new();

        private string BuildKey(int examId, int studentId)
            => $"{examId}_{studentId}";

        public void Enqueue(int examId, int studentId, DateTime deadline)
        {
            lock (_lock)
            {
                var key = BuildKey(examId, studentId);
                if (_keys.Contains(key))
                    return;

                _queue.Enqueue(
                    new ExamSubmitJob(examId, studentId, deadline),
                    deadline
                );

                _keys.Add(key);
            }
        }

        public bool TryDequeueExpired(DateTime now, out ExamSubmitJob? job)
        {
            lock (_lock)
            {
                if (_queue.Count == 0)
                {
                    job = null;
                    return false;
                }

                var next = _queue.Peek();
                if (next.Deadline > now)
                {
                    job = null;
                    return false;
                }

                job = _queue.Dequeue();
                _keys.Remove(BuildKey(job.ExamId, job.StudentId));
                return true;
            }
        }

        public void Remove(int examId, int studentId)
        {
            lock (_lock)
            {
                _keys.Remove(BuildKey(examId, studentId));
            }
        }
    }
}
