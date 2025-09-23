import { useQuery } from '@tanstack/vue-query';
import { computed } from 'vue';

export const useAuth = () => {
  const query = useQuery({
    queryKey: ['auth'],
    queryFn: () => fetch('/api/spotify/profile').then((r) => r.json()),
    staleTime: Infinity,
    gcTime: Infinity,
    retry: false,
  });
  const isLoading = computed(() => query.isPending.value);
  const isAuthenticated = computed(() => query.isSuccess.value === true);

  return {
    query,
    isLoading,
    isAuthenticated,
  };
};
