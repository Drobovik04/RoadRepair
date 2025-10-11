export type KeySelector<T> = (item: T) => unknown;

export function filterByQuery<T>(
  items: T[],
  query: string,
  keySelectors: KeySelector<T>[]
): T[] {
  const trimmed = query?.toString().trim().toLowerCase();
  if (!trimmed) return items;

  return items.filter((item) => {
    return keySelectors.some((select) => {
      try {
        const value = select(item);
        if (value == null) return false;
        if (typeof value === "string") {
          return value.toLowerCase().includes(trimmed);
        }
        if (typeof value === "number" || value instanceof Date) {
          return String(value).toLowerCase().includes(trimmed);
        }
        if (Array.isArray(value)) {
          return value
            .map((v) => (v == null ? "" : String(v).toLowerCase()))
            .some((v) => v.includes(trimmed));
        }
        return String(value).toLowerCase().includes(trimmed);
      } catch {
        return false;
      }
    });
  });
}


